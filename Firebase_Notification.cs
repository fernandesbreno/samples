using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        /*
         *Testes de envio de notificação 
         */
        public static void Main(string[] args)
        {
            string title = "Esse é o titutlo";
            string body = "Essa é a mensagem da notificação";
            //id e status do pedido  
            var data = new { status_pedido = 1, id_status= "810aa3ed-2aa8-420d-b17f-a99100ef37c5" };
            //Token do dispositivo exemplo
            var tokens = new string[] { "e1vpHKJ6n2k:APA91bEHZSuKC0oGurKKzFNJoms2MtALGQpJdGfTYx8tDA55CmP2GESzDq_PhSjAAyAzgL9B0PuwluTl-9L_rkfVmLkgNlW2oW6UTlKEpBy4wnYSf-E5DDuS6o5D9zAZavU294amqFvE" };


            var pushSent = LogicaEnvioNotificacao.EnviarNotificacao(tokens, title, body, data);
            Console.WriteLine("Notificação enviada! " + pushSent.Result);

        }
    }

    class LogicaEnvioNotificacao
    {
        //Firebase API URL
        private static Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");

        //Chave do servidor
        private static string ServerKey = "YOUR-SERVER-KEY";

        public static async Task<bool> EnviarNotificacao(string[] tokens, string titulo, string msg, object data)
        {
            //Flag de envio
            bool _enviado = false;

            //Criação do objeto da notificação
            var notificacao = new Message()
            {
                priority = "normal",
                notification = new Notification()
                {

                    title = titulo,
                    text = msg
                },
                data = data,
                registration_ids = tokens
            };

            //Objeto para Json (usando Newtonsoft.Json)
            string jsonMessage = JsonConvert.SerializeObject(notificacao);


            //Criando requisição para o Firebase API
            var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

            request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.SendAsync(request);
                _enviado = result.IsSuccessStatusCode;
            }


            return _enviado;
        }

    }

    //Modelos
    public class Message
    {
        public string[] registration_ids { get; set; }
        public string priority { get; set; }
        public Notification notification { get; set; }
        public object data { get; set; }
    }

    public class Notification
    {
        public string title { get; set; }
        public string text { get; set; }
    }
}



