/*
    Javascript file for implementing Leaflet mapping software    

    Version 11.8.24
*/


// Create a map that overlooks SHSU by default
let map = L.map('map').setView({ lon: -95.5474, lat: 30.7143 }, 15);


// Tilesets
// #region

// OpenStreetMaps tile set (DEFAULT)
var openMapsLayer = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
    {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }
);

// Satellite tile set
var satellite = L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
    attribution: 'Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community'
});

// #endregion


// [Basic Map Functions]
// #region 

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
export function add_marker(lat, lon, name, desc, img) {
    var marker = L.marker([lat, lon])
        .addTo(map)
        .on('click', () => { update_info(lat, lon, name, desc, img); });
}

// #endregion


export function get_coords() {
    return currPos;
}

export function get_route_coords() {
    return routePos;
}

let routed = null;

export function is_navigated() {
    return routed != null;
}

export function reset_navigation() {

    if (routed != null) {
        map.removeControl(routed);
        routed = null;
    }
    
}


let userMarker, userCircle, zoomed, position;


// Icons
// #region

/** The icon representing the user */
const userIcon = L.icon(
    {
        iconUrl: './images/Walkman_orange.png',
        iconSize: [50, 70]
    }
);

//#endregion


// Navigation
// #region

/**
 *  Code ran upon successfully getting the user's position
 * @param {any} pos
 */
function success(pos) {

    position = pos;     // Update the position of the user

    // Get the latitude and longitude of the user's lacation
    const lat = position.coords.latitude;
    const lon = position.coords.longitude;
    const accuracy = position.coords.accuracy;

    // Remove any previous user markers
    if (userMarker) {
        map.removeLayer(userMarker);
        map.removeLayer(userCircle);
    }

    // Add the user to the map
    userMarker = L.marker([lat, lon], {icon: userIcon}).addTo(map);
    userCircle = L.circle([lat, lon], { radius: accuracy }).addTo(map);

    // Zoom in on the user's location
    if (!zoomed) {
        zoomed = map.fitBounds(userCircle.getBounds());
    }
}

/**
 *  Code ran upon failing to get the user's position
 * @param {any} err
 */
function error(err) {

    if (err.code == 1) {
        alert("Please allow GPS access.");
    }
    else {
        alert("Failure to retrieve user location.");
    }

}

/**
 * Creates a route from the user to the building
 * 
 * @param {any} bLat The desired building's latitude
 * @param {any} bLon The desired building's longitude
 */
export function navigate_user(bLat, bLon) {
    // Route the user to the selected building
    // =======================================

    const uLat = position.coords.latitude;
    const uLon = position.coords.longitude;

    // Remove nav route if there already is one
    if (routed) {
        map.removeControl(routed);
    }

    // Create and add the route to the map
    routed = L.Routing.control({
        waypoints: [
            L.latLng(uLat, uLon),
            L.latLng(bLat, bLon)
        ]
    }).addTo(map);

    routed.hide();  // Hides itinerary

    routePos = new Array();
    routePos[0] = bLat;
    routePos[1] = bLon;
    // =======================================
}

// #endregion

let currPos, routePos;

/**
 * Updates the information box by the map
 * 
 * @param {any} name The building's name
 * @param {any} desc The building's description
 * @param {any} img The building's image source
 */
function update_info(lat, lon, name, desc, img) {

    // Update the selected position
    currPos = new Array();
    currPos[0] = lat;
    currPos[1] = lon;

    // Gets the associated information box to render the mark building info in
    var parent = document.getElementById("markerInfo");

    // Reset the information box
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }

    // Create and add the title to the information box
    var title = document.createElement("h2");
    title.innerHTML = "<text style=\"font-size: calc(1rem + 0.9vw)\">" + name + "</text>";
    title.style = "border-bottom: 3px solid black";
    parent.appendChild(title);

    // Create and add main body content container
    var contentContainer = document.createElement("div");
    contentContainer.style = "width: 100%; height: 91%; overflow: hidden auto;";

    // Create and add image container
    var imgContainer = document.createElement("div");
    imgContainer.style = "width: 100%; text-align: center; margin-top: 20px; margin-bottom: 20px; ";

    // Create and add the image to the image container, add it to the main content container
    var image = document.createElement("img");
    image.src = img;
    image.alt = name;
    image.style = "width: 90%;";
    imgContainer.appendChild(image);
    contentContainer.appendChild(imgContainer);

    // Create and add the description to the information box
    var body = document.createElement("p");
    body.innerHTML = desc;
    contentContainer.appendChild(body);

    // Add the content to the parent div
    parent.appendChild(contentContainer);
}

