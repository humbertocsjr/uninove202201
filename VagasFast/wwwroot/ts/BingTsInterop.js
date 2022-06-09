/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
var bingMap;
var map;
var cLat;
var cLon;
var BingMap = /** @class */ (function () {
    function BingMap(latitude, longitude) {
        this.map = new Microsoft.Maps.Map('#mapa', {
            center: new Microsoft.Maps.Location(latitude, longitude),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 15
        })
            ;

        var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(cLat, cLon), {
            icon: '<svg xmlns:svg="http://www.w3.org/2000/svg"  xmlns="http://www.w3.org/2000/svg"  xmlns: xlink = "http://www.w3.org/1999/xlink"    width="25" height="40" >  <circle cx="12.5" cy="14.5" r="10" fill="{color}"/>      </svg> ',
            color: 'blue',
            anchor: new Microsoft.Maps.Point(latitude, longitude)
        });
        //bingMap.entities.push(pushpin);

        this.map.entities.push(pushpin);

    }
    return BingMap;
}());
function carregarMapa(latitude, longitude) {
    bingMap = new BingMap(latitude, longitude);
    cLat = latitude;
    cLon = longitude;
}
function adicionarPonto(latitude , longitude ) {
    
    var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(cLat, cLon), {
        icon: '<svg xmlns:svg="http://www.w3.org/2000/svg"  xmlns="http://www.w3.org/2000/svg"  xmlns: xlink = "http://www.w3.org/1999/xlink"    width="25" height="40" >  <circle cx="12.5" cy="14.5" r="10" fill="{color}"/>      </svg> ',
        color: 'blue',
        anchor: new Microsoft.Maps.Point(latitude, longitude)
        });
      //bingMap.entities.push(pushpin);

    //this.map.entities.push(pushpin);

    var infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Point(latitude, longitude), {
        title: 'Vaga',
        description: 'Disponivel'
    });
    infobox.setMap(map);

}
//# sourceMappingURL=BingTsInterop.js.map