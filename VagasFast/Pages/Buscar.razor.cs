using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace VagasFast.Pages
{
	public class BuscarBase : ComponentBase
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var localizacao = await Geolocation.GetLocationAsync();
            await JSRuntime.InvokeAsync<Task>("carregarMapa", localizacao.Latitude, localizacao.Longitude);
            await JSRuntime.InvokeAsync<Task>("adicionarPonto", localizacao.Latitude, localizacao.Longitude );
        }
    }
}

