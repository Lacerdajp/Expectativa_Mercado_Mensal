using Expectativa_do_Mercado_Mensal.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Expectativa_do_Mercado_Mensal.Service
{
    internal class BacenService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<ExpectativasMercado>> GetExpectativasAsync(string indicador,DateTime dateInicio,DateTime dateFim)
        {
            var request = new HttpRequestMessage();
            if (indicador.Equals("Selic"))
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativasMercadoSelic?$format=json&$filter=Data ge '" + dateInicio.ToString("yyyy-MM-dd") + "' and Data le '" + dateFim.ToString("yyyy-MM-dd") + "' and Indicador eq '" + indicador + "'");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativaMercadoMensais?$format=json&$filter=Data ge '"+dateInicio.ToString("yyyy-MM-dd") + "' and Data le '"+dateFim.ToString("yyyy-MM-dd") + "' and Indicador eq '"+ indicador+"'");
            }
            

            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            try { 
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(json);

                JArray jsonArray = (JArray)jsonObject["value"];
                List<ExpectativasMercado> result=new List<ExpectativasMercado>();
                foreach (JObject item in jsonArray)
                {
                    if (item.ContainsKey("Reuniao"))
                    {
                        JToken value = item["Reuniao"];
                        item.Remove("Reuniao");
                        item["DataReferencia"] = value;
                    }
                    result.Add(item.ToObject<ExpectativasMercado>());
                }
                return result;
            }
                MessageBox.Show(response.StatusCode.ToString());
                return new List<ExpectativasMercado>();

            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
                return new List<ExpectativasMercado>();

            }
        }
    }
}
