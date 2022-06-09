/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />

let bingMap: BingMap;

class BingMap {
    map: Microsoft.Maps.Map;

    constructor(latitude : any, longitude : any) {
        this.map = new Microsoft.Maps.Map('#mapa', {
            center: new Microsoft.Maps.Location(latitude, longitude),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 15
        });
    }
}

function carregarMapa(latitude: any, longitude: any): void {
    bingMap = new BingMap(latitude, longitude);
}

function adicionarPonto(latitude: any, longitude: any): void {

}