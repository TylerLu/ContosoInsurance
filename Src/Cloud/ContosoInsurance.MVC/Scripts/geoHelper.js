;
GeoHelper = {};
GeoHelper.BingMap = {
    key:'AgWV4amBPg2dzbgGPB8Ya5GDfGqBnueTJUfu1oAAilDje_vXB0RpKGF4mDabOmwB',
    displayPin: function (latitude,longitude,bingMap$Id,address$Id) {
        var map = new Microsoft.Maps.Map($("#"+bingMap$Id).get(0), {
            credentials: GeoHelper.BingMap.key,
        });
        Microsoft.Maps.loadModule('Microsoft.Maps.Search', function () {
            var searchManager = new Microsoft.Maps.Search.SearchManager(map);
            var reverseGeocodeRequestOptions = {
                location: new Microsoft.Maps.Location(latitude, longitude),
                callback: function (answer, userData) {
                    map.setView({ bounds: answer.bestView });
                    map.entities.push(new Microsoft.Maps.Pushpin(reverseGeocodeRequestOptions.location));
                    $("#"+address$Id).val(answer.address.formattedAddress);
                }
            };
            searchManager.reverseGeocode(reverseGeocodeRequestOptions);
        });
    }
};