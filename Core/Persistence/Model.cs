using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using StarterPack.Core.Persistence;
using StarterPack.Core.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace StarterPack.Core.Persistence
{
    public abstract partial class Model<T> where T :  Model<T>
    {
        public virtual long? Id { get; set; }

        [Required, FromQuery(Name = "created_at")]
        public virtual DateTimeOffset? CreatedAt { get; set; }

        [Required, FromQuery(Name = "updated_at")]
        public virtual DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Lista que deve conter os attributos que são preenchidos através do metodo FillAttributes e UpdateAttributes
        /// "*" coringa para representar todos os atributos
        /// </summary>
        [NotMapped]
        public virtual List<string> Fill { get; set; }

        /// <summary>
        /// Lista que deve conter os attributos que não vão ser preenchidos através do metodo FillAttributes e UpdateAttributes
        /// Essa lista tem prioridade em relação a lista Fill. Nesta o coringa "*" não funciona
        /// </summary>
        [NotMapped]
        public virtual List<string> DontFill { get; set; }

        protected readonly DbContext context;
        protected DbSet<T> entities;

        /// <summary>
        /// Contrutor que define como contexto de dados o contexto padrão da requisição,
        /// inicializa as coleções Fill e DontFill e define as datas de CreatedAt e UpdatedAt como a data corrente
        /// </summary>
        public Model() : this(getContext()) {}

        /// <summary>
        /// Contrutor que define com contexto de dados passado por parâmetro,
        /// inicializa as coleções Fill e DontFill e define as datas de CreatedAt e UpdatedAt como a data corrente
        /// </summary>
        /// <param name="context">Contexto de dados a ser utilizado para processar transações com o banco</param>
        public Model(DbContext context){
            this.context = context;
            entities = context.Set<T>();
            this.Fill = new List<string> {};
            this.DontFill = new List<string> {};
            this.CreatedAt = this.UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Adiciona um model ao contexto de forma que seja possível recuperar as entidades relacionadas
        /// Veja o exemplo: https://docs.microsoft.com/en-us/ef/core/querying/related-data#explicit-loading
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static EntityEntry<T> Entry(T model) {
            EntityEntry<T> entry = getContext().Entry<T>(model);
            return entry;
        }

        /// <summary>
        /// Adiciona um model já existente no banco e não o marca como modificado. A partir das primeira modificação para a ter o estado como modificado
        /// Veja o exemplo: https://docs.microsoft.com/en-us/ef/core/querying/related-data#explicit-loading
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static EntityEntry<T> Attach(T model) {
            EntityEntry<T> entry = getContext().Attach<T>(model);
            return entry;
        }

        /// <summary>
        /// Carrega os relacionamentos especificados de uma entidade
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public EntityEntry<T> LoadRelations(Expression<Func<T, bool>> predicate) {
            var expression = predicate.Body as NewExpression;
            EntityEntry<T> entry = getContext().Entry<T>((T)this);

            foreach (MemberExpression a in expression.Arguments) {
                var propertyName = a.Member.Name;
                if(this.GetType().IsAssignableFrom(typeof(IList))){
                    entry.Collection(propertyName);
                }
                else {
                    entry.Reference(propertyName);
                }
            }

            return entry;
        }


        /// <summary>
        /// Recupera  a instância de um model pela PK id.
        /// Por padrão é retornada com treck ATIVO (mudanças serão refletidas no banco quando chamado executado SaveChanges),
        /// podendo este track ser desativado passando o último parâmetro como false
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tracked">Define se a entidade recuperada deve manter o track ativo (mudanças serão refletidas no banco quando chamado executado SaveChanges) [padrão true]</param>
        /// <returns></returns>
        public static T Get(long id, bool tracked = true){
            if(tracked) {
                return getEntities().SingleOrDefault(s => s.Id == id);
            }
            return getEntities().AsNoTracking().SingleOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Recupera  uma coleção de models.
        /// Por padrão é retornada com treck INATIVO (mudanças NÃO serão refletidas no banco quando chamado executado SaveChanges),
        /// podendo este track ser ativado passando o parâmetro tracked como true
        /// </summary>
        /// <param name="tracked">Define se a entidade recuperada deve manter o track ativo (mudanças serão refletidas no banco quando chamado executado SaveChanges) [padrão false]</param>
        /// <returns>List<T></returns>
        public static List<T> GetAll(bool tracked = false) {
            if(tracked) {
                return getEntities().Fetch();
            }
            return getEntities().AsNoTracking().Fetch();
        }

        /// <summary>
        /// Recupera  uma coleção de models por critérios passados como parâmetro
        /// Por padrão é retornada com treck INATIVO (mudanças NÃO serão refletidas no banco quando chamado executado SaveChanges),
        /// podendo este track ser ativado passando o parâmetro tracked como true
        /// </summary>
        /// <param name="predicate">Expressão linq com consições para a recuperação de models</param>
        /// <param name="tracked">Define se a entidade recuperada deve manter o track ativo (mudanças serão refletidas no banco quando chamado executado SaveChanges) [padrão false]</param>
        /// <returns>List<Model></returns>
        public static List<T> FindBy(Expression<Func<T, bool>> predicate, bool tracked = false) {
            if(tracked) {
                return Where(predicate).Fetch();
            }

            return Where(predicate).AsNoTracking().Fetch();
        }

        /// <summary>
        /// Recupera o objeto que representa a query para o modelo
        /// </summary>
        /// <param name="tracked">Define se os models recuperados a partir da query retornada deverão manter o track ativo (mudanças serão refletidas no banco quando chamado executado SaveChanges) [padrão false]</param>
        /// <returns></returns>
        public static IQueryable<T> Query(bool tracked = false) {
            if(tracked) {
                return getEntities();
            }
            return getEntities().AsNoTracking();
        }

        /// <summary>
        /// Executa uma query raw e retorna um conjunto de models
        /// </summary>
        /// <param name="sql">String sql a ser executada com os devidos placeholders dos parâmetros (caso contenha parâmetros)</param>
        /// <param name="parameters">Parâmetros a serem inseridos na query sql</param>
        /// <returns></returns>
        public static List<T> TypedRawSql(string sql, params object[] parameters) {
           return getEntities().FromSql(sql, parameters).ToList();
        }

        /// <summary>
        /// Executa uma query raw e retorna um RelationalDataReader (que pode conter qualquer conjunto de dados)
        /// </summary>
        /// <param name="sql">String sql a ser executada com os devidos placeholders dos parâmetros (caso contenha parâmetros)</param>
        /// <param name="parameters">Parâmetros a serem inseridos na query sql</param>
        /// <returns>RelationalDataReader - Para ler os resultados execute: var dr = RawSql([parameters]); while (dr.Read()) { Console.Write("{0}\t{1}\t{2} \n", dr[0], dr[1], dr[2]);}</returns>
        public static RelationalDataReader RawSql(string sql, params object[] parameters) {
           return getContext().Database.ExecuteSqlQuery(sql, parameters);
        }


        /// <summary>
        /// Recupera o objeto query do modelo com critérios passados como parâmetro
        /// Por padrão é retornada com treck INATIVO (mudanças NÃO serão refletidas no banco quando chamado executado SaveChanges),
        /// podendo este track ser ativado passando o parâmetro tracked como true
        /// </summary>
        /// <param name="predicate">Expressão linq com consições para a recuperação de models</param>
        /// <param name="tracked">Define se a entidade recuperada deve manter o track ativo (mudanças serão refletidas no banco quando chamado executado SaveChanges) [padrão false]</param>
        /// <returns>Objeto query que pode ser utilizada para fazer uma consulta no banco executando query.Fetch()</returns>
        public static IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool tracked = false) {
            return Query(tracked).Where(predicate);
        }


        /// <summary>
        /// Recupera o objeto query do modelo com o critério PK id = [parametro id]
        /// Por padrão é retornada com treck ATIVO (mudanças NÃO serão refletidas no banco quando chamado executado SaveChanges),
        /// podendo este track ser desativado passando o parâmetro tracked como false
        /// </summary>
        /// <param name="predicate">Expressão linq com consições para a recuperação de models</param>
        /// <param name="tracked">Define se a entidade recuperada deve manter o track ativo (mudanças serão refletidas no banco quando chamado executado SaveChanges) [padrão true]</param>
        /// <returns>Objeto query que pode ser utilizada para fazer uma consulta no banco executando query.Fetch()</returns>
        public static IQueryable<T> WhereId(long id, bool tracked = true) {
            return Where(s => s.Id == id, tracked);
        }

        /// <summary>
        /// Salva todas os dados marcados como pendentes no banco de dados
        /// Se utilizado em conjunto com operações em lote como, por exemplo, Model.Save(false), Model.Update(false), Model.Delete(false)
        /// permite aplicar efetivamente todas as mudanças de uma vez só, sendo ùtil para transações atômicas
        /// </summary>
        public static void SaveChanges(DbContext context = null) {
            var _context = context == null ? getContext() : context;
            _context.SaveChanges();
        }

        /// <summary>
        /// Salva as modificações em um model. Por padrão as mudanças são efetivamente salvas no banco no momento desta chamada,
        /// podendo ser passado o parâmetro applyChanges como false para que essa mudança só seja efetivada em momento posterior
        /// executando o método CommitChanges
        /// </summary>
        /// <param name="applyChanges">Sendo passado como false evita a aplicação efetiva da mudança no banco, que nesse caso deve ser executada usando o método CommitChanges</param>
        public void Save(bool applyChanges = true) {
            if(this.Id != null) {
                Update(applyChanges);
            }
            else {
                Add((T) this);

                if(applyChanges) {
                    getContext().SaveChanges();
                }
            }
        }

        /// <summary>
        /// Adiciona um model e já o marca como modificado.
        /// </summary>
        /// <param name="entity"></param>
        public static void Add(T entity) {
            var context = getContext();
            getEntities(context).Add(entity);
        }

        /// <summary>
        /// Exclui um registro pela PK Id. Por padrão essa exclusão já é persistida no banco.
        /// Se for passado o segundo parâmetro (applyChanges) como false, não salva, mas deixa marcado como modificado
        /// podendo ser executado o método SaveChanges
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applyChanges">Se for passado como false, não salva, mas deixa marcado como modificado</param>
        public static void Delete(long id, bool applyChanges = true) {
            var context = getContext();
            T model = Model<T>.Get(id);

            getEntities(context).Remove(model);

            if(applyChanges) {
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Exclui um ou mais registros a partir dos critérios passados. Por padrão essa operação já é persistida no banco.
        /// Se for passado o segundo parâmetro (applyChanges) como false, não salva, mas deixa marcado como modificado
        /// podendo ser executado o método SaveChanges
        /// </summary>
        /// <param name="predicate">Expressão lambda com condições para exclusão</param>
        /// <param name="applyChanges">Se for passado como false, não salva, mas deixa marcado como modificado</param>
        public static void Delete(Expression<Func<T, bool>> predicate = null, bool applyChanges = true) {
            var context = Model<T>.getContext();

            if(predicate != null)
                context.RemoveRange(Model<T>.Where(predicate).Where(m => m.Id > 0));
            else
                context.RemoveRange(Model<T>.Where(m => m.Id > 0));

            if(applyChanges) {
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Atualiza um modelo (atualizando automaticamente o campo UpdatedAt). Por padrão essa operação já é persistida no banco.
        /// Se for passado o segundo parâmetro (applyChanges) como false, não salva, mas deixa marcado como modificado
        /// podendo ser executado o método SaveChanges
        /// </summary>
        /// <param name="applyChanges">Se for passado como false, não salva, mas deixa marcado como modificado</param>
        public virtual void Update(bool applyChanges = true) {
            var context = getContext();
            getEntities(context).Update((T)this);
            UpdatedAt = DateTime.Now;

            if(applyChanges) {
                context.SaveChanges();
            }
        }
    }
}
