﻿/*
    Javascript file for implementing Leaflet mapping software    

    Version 10.27.24
*/


// Create a map that overlooks SHSU by default
let map = L.map('map').setView({ lon: -95.5474, lat: 30.7143 }, 15);

// OpenStreetMaps tile set (DEFAULT)
var openMapsLayer = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
    {
        maxZoom: 20,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }
);

// Satellite tile set
var satellite = L.tileLayer('https://tiles.stadiamaps.com/tiles/alidade_satellite/{z}/{x}/{y}{r}.{ext}',
    {
        maxZoom: 20,
        attribution: '&copy; CNES, Distribution Airbus DS, © Airbus DS, © PlanetObserver (Contains Copernicus Data) | &copy; <a href="https://www.stadiamaps.com/" target="_blank">Stadia Maps</a> &copy; <a href="https://openmaptiles.org/" target="_blank">OpenMapTiles</a> &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        ext: 'jpg'
    }
);


// Loads the map
export function load_map(buildingList) {

    // Add the default map tileset
    openMapsLayer.addTo(map);

    // Add user location to the map
    navigator.geolocation.watchPosition(success, error);

    return "";
}

// Toggle from default and sattelite
export function switch_map(mapName) {

    if (mapName == "Satellite")    // When on default map
    {
        map.addLayer(satellite);
        map.removeLayer(openMapsLayer);
    }
    else if (mapName == "Default") // When on satellite map
    {
        map.addLayer(openMapsLayer);    
        map.removeLayer(satellite);
    }

}

// Adds a marker to the map
export function add_marker(lat, long, name, desc, img) {

    var marker = L.marker([lat, long])
        .addTo(map)
        .on('click', () => { update_info(name, desc, img); });

}


let userMarker, userCircle, zoomed;

const userIcon = L.icon(
    {
        iconUrl: './images/Walkman_orange.png',
        iconSize: [50, 70]
    }
);

function success(pos) {

    const lat = pos.coords.latitude;
    const lon = pos.coords.longitude;
    const accuracy = pos.coords.accuracy;

    if (userMarker) {
        map.removeLayer(userMarker);
        map.removeLayer(userCircle);
    }

    userMarker = L.marker([lat, lon], {icon: userIcon}).addTo(map);
    userCircle = L.circle([lat, lon], { radius: accuracy }).addTo(map);

    if (!zoomed) {
        zoomed = map.fitBounds(userCircle.getBounds());
    }
    
}

function error(err) {

    if (err.code == 1) {
        alert("Please allow GPS access.");
    }
    else {
        alert("Failure to retrieve user location.");
    }

}

// Updates the information box by the map
function update_info(name, desc, img) {

    // Gets the associated information box to render the mark building info in
    var parent = document.getElementById("markerInfo");

    // Reset the information box
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }

    // Add the title to the information box
    var title = document.createElement("h2");
    title.innerHTML = "<text>" + name + "</text>";
    title.style = "border-bottom: 3px solid black";
    parent.appendChild(title);

    // Main body content container
    var contentContainer = document.createElement("div");
    contentContainer.style = "width: 100%; height: 80vh; overflow: hidden auto;";

    // Add image container
    var imgContainer = document.createElement("div");
    imgContainer.style = "width: 100%; text-align: center; margin: 20px;";

    // Add the image to the image container, add it to the main content container
    var image = document.createElement("img");
    image.src = img;
    image.alt = name;
    image.style = "height: 375px;";
    imgContainer.appendChild(image);
    contentContainer.appendChild(imgContainer);

    // Add the description to the information box
    var body = document.createElement("p");
    body.innerHTML = desc;
    contentContainer.appendChild(body);

    parent.appendChild(contentContainer);
    
}