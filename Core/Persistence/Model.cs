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

namespace StarterPack.Core.Models
{
    public abstract class Model<T> where T :  Model<T>
    {
        public virtual long? Id { get; set; }
        
        [FromQuery(Name = "created_at")]
        public virtual DateTime? CreatedAt { get; set; }

        [FromQuery(Name = "updated_at")]
        public virtual DateTime? UpdatedAt { get; set; }

        [NotMapped]
        protected virtual List<string> Fill { get; set; }

        [NotMapped]
        protected virtual List<string> DontFill { get; set; }

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
        public Model(DbContext context)
        {           
            this.context = context;
            entities = context.Set<T>();             
            this.Fill = new List<string> {}; 
            this.DontFill = new List<string> {};
            this.CreatedAt = this.UpdatedAt = DateTime.Now;           
        }

        /// <summary>
        /// Recupera o contexto de dados. Por padrão é recuperado o DefaultDbContext. 
        /// Caso o model vá utilizar um contexto de dados (conexão/banco) diferente deve 
        /// implementar o método protected "new static DbContext getContext() { /* return your db context */}"
        /// </summary>
        /// <returns></returns>
        protected static DbContext getContext() {            
            return DbContextAccessor.GetContext(typeof(DefaultDbContext));
        }        

        /// <summary>
        /// Recupera a referência às entidades de dados
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static DbSet<T> getEntities(DbContext context = null) {
            var _context = context == null ? getContext() : context;
            return _context.Set<T>();
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
        /// Adiciona um model não rastreado ao contexto de forma que este torna-se rastreado e as mudanças neste podem ser salvas no banco
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
        /// Recupera  a instância de um model pela FK id. 
        /// Por padrão é retornada com treck ATIVO (mudanças serão refletidas no banco quando chamado executado SaveChanges), 
        /// podendo este track ser desativado passando o último parâmetro como false
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tracked">Define se a entidade recuperada deve manter o track ativo (mudanças serão refletidas no banco quando chamado executado SaveChanges) [padrão true]</param>
        /// <returns></returns>
        public static T Get(long id, bool tracked = true)
        {      
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
        /// Recupera o objeto query do modelo com o critério fk id = [parametro id]
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
        public static void CommitChanges() {
            getContext().SaveChanges();
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
                Save((T)this, applyChanges);            
            }            
        }

        /// <summary>
        /// Salva as modificações de um model passado como parâmetro. Por padrão as mudanças são efetivamente salvas no banco no momento desta chamada,
        /// podendo ser passado o parâmetro applyChanges como false para que essa mudança só seja efetivada em momento posterior
        /// executando o método CommitChanges
        /// </summary>
        /// <param name="applyChanges">Sendo passado como false evita a aplicação efetiva da mudança no banco, que nesse caso deve ser executada usando o método CommitChanges</param>
        public static void Save(T entity, bool applyChanges = true) {
            var context = getContext();            
            getEntities(context).Add(entity);

            if(applyChanges) {
                context.SaveChanges();
            }            
        }
        
        public static void Add(T entity) {
            var context = getContext();            
            getEntities(context).Add(entity);
        }

        public static void Delete(long id, bool applyChanges = true) {    
            var context = getContext();      
            T model = Model<T>.Get(id);   
          
            getEntities(context).Remove(model);

            if(applyChanges) {
                context.SaveChanges();
            }
        }

        public void Delete(bool applyChanges = true) {
            entities.Remove((T)this);
            
            if(applyChanges) {
                context.SaveChanges();
            }
        }

        public static void Delete(Expression<Func<T, bool>> predicate = null) {
            var context = Model<T>.getContext();  
            context.RemoveRange(Model<T>.Where(predicate).Where(m => m.Id > 0));
            context.SaveChanges();
        }

        public static void DeleteAll() {
            var context = Model<T>.getContext();  
            context.RemoveRange(Model<T>.Query().Where(m => m.Id > 0));
            context.SaveChanges();
        }

        public virtual void Update(bool applyChanges = true) { 
            var context = getContext();            
            getEntities(context).Update((T)this);
            UpdatedAt = DateTime.Now;

            if(applyChanges) {
                context.SaveChanges();
            }
        } 

        public virtual void UpdateAttributes(ExpandoObject updatedProperties) 
        {
            UpdateAttributes((T) this, updatedProperties);
        }

        public static void UpdateAttributes(long id, ExpandoObject updatedProperties)
        {            
            T model = Get(id);
            UpdateAttributes(model, updatedProperties);
        } 

        public void MergeAttributes(T updatedProperties) 
        {
            foreach (PropertyInfo property in updatedProperties.GetType().GetProperties())
            {   
                MergetProperty(this, property, property.GetValue(updatedProperties));
            }            
        }

        private static void UpdateAttributes(T model, ExpandoObject updatedProperties) 
        {            
            SetAttributes(ref model, updatedProperties);
            model.UpdatedAt = DateTime.Now;
            
            getContext().SaveChanges();
        }

        private static void SetAttributes(ref T model, ExpandoObject attributes) 
        {
            foreach (KeyValuePair<string, object> attribute in attributes)
            {
                String propertyName = StringHelper.SnakeCaseToTitleCase(attribute.Key);
                PropertyInfo property = model.GetType().GetProperty(propertyName);                

                if(property != null) {
                    MergetProperty(model, property, attribute.Value);
                }
            }            
        }        

        private static void MergetProperty(Model<T> model, PropertyInfo property, dynamic value) 
        {
            if(property.Name != "Id" 
            && !model.DontFill.Contains(property.Name) 
            && (model.Fill.Contains(property.Name) || model.Fill.Contains("*"))) {
                if(value != null) {
                    property.SetValue(model, value);              
                }
            }
        }                      
    }
   
}
