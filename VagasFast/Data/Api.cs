using System;
using System.Text.Json;

namespace VagasFast.Data
{
	public class Api
	{
		public Api()
		{
		}

		public static string Endereco => "http://10.0.2.10:5000/";

		public static string Token
		{
			get
			{
				return Preferences.Get("token", "");
			}
			set
			{
				Preferences.Set("token", value);
			}
		}

		public static bool TokenExiste => !string.IsNullOrEmpty(Token);

		public static async Task<T1> PostAs<T1>(string servico, Dictionary<string, string> valores)
        {
			var req = new HttpRequestMessage(HttpMethod.Post, (Endereco + servico));
			

			req.Content = new FormUrlEncodedContent(valores);


			var cliente = new HttpClient();
			var ret = await cliente.SendAsync(req);

			if(ret.IsSuccessStatusCode)
            {
				var conteudo = await ret.Content.ReadAsStreamAsync();
				var obj = await JsonSerializer.DeserializeAsync<T1>(conteudo);
				return obj;
            }
			else
            {
				throw new Exception("Ocorreu um erro ao tentar se conectar");
            }
		}

		public static async Task<IAsyncEnumerable<T1>> PostAsList<T1>(string servico, Dictionary<string, string> valores)
		{
			var req = new HttpRequestMessage(HttpMethod.Post, (Endereco + servico));


			req.Content = new FormUrlEncodedContent(valores);


			var cliente = new HttpClient();
			var ret = await cliente.SendAsync(req);

			if (ret.IsSuccessStatusCode)
			{
				var conteudo = await ret.Content.ReadAsStreamAsync();
				var obj = JsonSerializer.DeserializeAsyncEnumerable<T1>(conteudo);
				return obj;
			}
			else
			{
				throw new Exception("Ocorreu um erro ao tentar se conectar");
			}
		}
	}
}

