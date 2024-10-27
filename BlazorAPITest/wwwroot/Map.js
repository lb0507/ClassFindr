/*
    Javascript file for implementing Leaflet mapping software    

    Version 10.27.24
*/

// Loads the map
export function load_map() {

    // Create a map that overlooks SHSU by default
    let map = L.map('map').setView({ lon: -95.5474, lat: 30.7143 }, 15);

    // Add the actual map tile layer
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }
    ).addTo(map);

    return "";
}