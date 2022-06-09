/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
var bingMap;
var map;
var cLat;
var cLon;
var infobox;
var BingMap = /** @class */ (function () {
    function BingMap(latitude, longitude) {
        this.map = new Microsoft.Maps.Map('#mapa', {
            center: new Microsoft.Maps.Location(latitude, longitude),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 15
        })
            ;
        infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(latitude, longitude), {
            title: 'Vaga',
            description: 'Disponivel'
        });
        infobox.setMap(this.map);


    }
    return BingMap;
}());
function carregarMapa(latitude, longitude) {
    bingMap = new BingMap(latitude, longitude);
    cLat = latitude;
    cLon = longitude;
}
//# sourceMappingURL=BingTsInterop.js.map