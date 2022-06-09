using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VagasFast.Data;

namespace VagasFast.Pages
{
	public class BuscarBase : ComponentBase
    {
        protected List<Vaga> vagas = new List<Vaga>();
        protected string DescricaoValor { get; set; } = "";

        protected string Mensagem { get; set; } = "";
        protected int pos = 0;
        protected Location localizacao = null;

        [Inject]
        IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (vagas.Count == 0)
            {
                localizacao = localizacao ?? await Geolocation.GetLocationAsync();
                var dados = new Dictionary<string, string>();
                dados.Add("latitude", localizacao.Latitude.ToString());
                dados.Add("longitude", localizacao.Longitude.ToString());
                var vs = (await Api.PostAsList<Vaga>("vagas", dados)).GetAsyncEnumerator();

                while (await vs.MoveNextAsync())
                {
                    vagas.Add(vs.Current);
                }

            }
            await JSRuntime.InvokeAsync<Task>("carregarMapa", vagas[pos].Latitude, vagas[pos].Longitude);
        }


        protected async void proximo()
        {
            if ((pos+1) >= vagas.Count) pos = -1;
            pos++;
            await JSRuntime.InvokeAsync<Task>("carregarMapa", vagas[pos].Latitude, vagas[pos].Longitude);
        }

        protected async void anterior()
        {
            if ((pos - 1) == 0) pos = vagas.Count;
            pos--;
            await JSRuntime.InvokeAsync<Task>("carregarMapa", vagas[pos].Latitude, vagas[pos].Longitude);
        }
    }
}

