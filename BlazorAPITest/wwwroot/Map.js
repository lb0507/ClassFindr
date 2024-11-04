/*
    Javascript file for implementing Leaflet mapping software    

    Version 10.27.24
*/

// Create a map that overlooks SHSU by default
let map = L.map('map').setView({ lon: -95.5474, lat: 30.7143 }, 15);

// Loads the map
export function load_map(buildingList) {

    // Add the actual map tile layer
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }
    ).addTo(map);

    return "";
}

// Adds a marker to the map
export function add_marker(lat, long, name, desc) {

    var marker = L.marker([lat, long])
        .addTo(map)
        .bindPopup("<h5>" + name + "</h5>")
        .on('click', () => { update_info(name, desc); });

}

// Updates the information box by the map
function update_info(name, desc) {

    // Gets the associated information box to render the mark building info in
    var parent = document.getElementById("markerInfo");

    // Reset the information box
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }

    // Add the title to the information box
    var title = document.createElement("h2");
    title.innerHTML = "<text>" + name + "</text>";
    parent.appendChild(title);

    // Add the description to the information box
    var body = document.createElement("p");
    body.innerHTML = desc;
    parent.appendChild(body);
}