
using System.Reflection;
using Domain.Attributes;

namespace Domain.Model.Messaging {

    public class Response {
        public bool Success => Messages.All(x => x.MessageType != MessageType.Error);
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public Response(IEnumerable<Message> messages) {
            foreach (var message in messages) {
                Messages.Add(message);
            }
        }

        public Response() { }
    }

    public class Response<T> : Response {
        private readonly ICollection<T> _content = new List<T>();

        // Indien de lijst enkel uit nulls bestaat, wordt null gereturned
        public ICollection<T>? Content => !_content.All(i => i is null) ? _content : null;

        private Type GetInnerGenericType(Type type) {
            Type? innerType = type.GetGenericArguments().FirstOrDefault();
            return innerType is null ? type : GetInnerGenericType(innerType);
        }

        public Dictionary<string, string> LocalizedProperties {
            get {
                Dictionary<string, string> result = new();

                PropertyInfo[] propertyInfos = GetInnerGenericType(typeof(T)).GetProperties();

                foreach (PropertyInfo propertyInfo in propertyInfos) {
                    var res = (LocalizedDisplayNameAttribute?)propertyInfo.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(LocalizedDisplayNameAttribute));

                    if (res != null) {
                        result.Add(propertyInfo.Name, res.LocalizedDisplayName);
                    }
                }

                return result;
            }
        }


        public Response() { }

        public Response(T response) {
            _content.Add(response);
        }

        public Response(ICollection<T> values) {
            _content = values;
        }

        public Response<T> AddMessage(Message message) {
            Messages.Add(message);
            return this;
        }

        public Response<T> AddMessage(string message, MessageType type) {
            Messages.Add(new Message {
                Body = message,
                MessageType = type
            });
            return this;
        }

        public Response<T> AddError(string message) {
            AddMessage(message, MessageType.Error);
            return this;
        }

        public Response<T> AddError(Exception exception) {
            this.AddError(exception.GetType().Name + " >> " + exception.Message);

            if (exception.InnerException != null) {
                this.AddError(exception.InnerException);
            }
            return this;
        }



    }
}
