using System;
using System.Linq.Expressions;
using System.Reflection;

namespace StarterPack.Core.Validation
{
    public class ValidationResourceProvider
    {
        public static Func<Type, MemberInfo, LambdaExpression, string> DisplayNameResolver = (type, memberInfo, expression) => {
            // Aqui recuperamos o caminho completo da propriedade [ex.: Namespace.Class.Property]
            string fullName = $"attributes:{type}:{memberInfo.Name}".Replace(".",":");
            

            string value = Lang.Get(fullName);
            // Se a chave não foi encontrada tentamos somente pela classe e propriedade [Class.Property] (quando não é encontrada retorna a chave)
            if(value == fullName) {
                string classPropertyName = $"attributes:{type.Name}:{memberInfo.Name}";
                value = Lang.Get(classPropertyName);

                // Se a chave não foi encontrada tentamos somente pelo nome da propriedade (quando não é encontrada retorna a chave)
                if(value == classPropertyName) {                   
                    value = Lang.Get("attributes:"+memberInfo.Name);
                }
            }
            return value;
        };

        
        
        #region getters for the resource provider
        public static string notempty_error {
            get { 
                return Lang.Get("validation:notempty_error");
            }
        }
        public static string email_error {
            get { 
                return Lang.Get("validation:email_error");
            }
        }
        public static string equal_error {
            get { 
                return Lang.Get("validation:equal_error");
            }
        }
        public static string exact_length_error {
            get { 
                return Lang.Get("validation:exact_length_error");
            }
        }
        public static string exclusivebetween_error {
            get { 
                return Lang.Get("validation:exclusivebetween_error");
            }
        }
        public static string greaterthan_error {
            get { 
                return Lang.Get("validation:greaterthan_error");
            }
        }
        public static string greaterthanorequal_error {
            get { 
                return Lang.Get("validation:greaterthanorequal_error");
            }
        }
        public static string inclusivebetween_error {
            get { 
                return Lang.Get("validation:inclusivebetween_error");
            }
        }
        public static string length_error {
            get { 
                return Lang.Get("validation:length_error");
            }
        }
        public static string lessthan_error {
            get { 
                return Lang.Get("validation:lessthan_error");
            }
        }
        public static string lessthanorequal_error {
            get { 
                return Lang.Get("validation:lessthanorequal_error");
            }
        }
        public static string notequal_error {
            get { 
                return Lang.Get("validation:notequal_error");
            }
        }
        public static string notnull_error {
            get { 
                return Lang.Get("validation:notnull_error");
            }
        }
        public static string predicate_error {
            get { 
                return Lang.Get("validation:predicate_error");
            }
        }
        public static string regex_error {
            get { 
                return Lang.Get("validation:regex_error");
            }
        }

        #endregion
        
    }

}