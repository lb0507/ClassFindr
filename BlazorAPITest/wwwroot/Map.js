/*
    Javascript file for implementing Leaflet mapping software    

    Version 11.8.24
*/


// Create a map that overlooks SHSU by default
let map = L.map('map').setView({ lon: -95.5474, lat: 30.7143 }, 15);

let isDisplayingAccuracy = false;   // Holds if the map shows location accuracy or not

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

    console.log("Hey! Why are you looking in here, nosy much?");    // Easter egg for the nosy type

    // Add the default map tileset
    openMapsLayer.addTo(map);

    // Add user location to the map
    navigator.geolocation.watchPosition(success, error);

    // Initialize the information box with it's associated text
    var parent = document.getElementById("markerInfo");


    // Create and add the message to the information box
    var message = document.createElement("text");
    message.innerHTML = "<br/><em style=\"padding-left: 10px;\">Please select a building to view...</em>"
    message.style = "color: grey;";
    parent.appendChild(message);

    return "";
}

/** Toggle from default and sattelite map views */
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

/** Adds a marker to the map */
export function add_marker(lat, lon, name, desc, img) {
    var marker = L.marker([lat, lon])
        .addTo(map)
        .on('click', () => { update_info(lat, lon, name, desc, img); });
}

/** Gets the current coordinates of the user */
export function get_coords() {

    return currPos;
}

/** Gets the end coordinates of the route */
export function get_route_coords() {
    return routePos;
}

let routed = null;  // Object that holds the user's navigation route

/** Returns whether or not if the user has begun navigation or not already */
export function is_navigated() {
    return routed != null;
}

/** Resets the user's route */
export function reset_navigation() {

    if (routed != null) {
        map.removeControl(routed);
        routed = null;
    }
    
}

/**
 * Shows/Hides the blue circle that represents accuracy
 * @param {any} isShown
 */
export function show_accuracy(isShown) {

    if (isShown)
    {
        const lat = position.coords.latitude;
        const lon = position.coords.longitude;
        const accuracy = position.coords.accuracy;
        userCircle = L.circle([lat, lon], { radius: accuracy }).addTo(map);
    }
    else
    {
        map.removeLayer(userCircle);
        userCircle = null;
    }

    isDisplayingAccuracy = isShown;
}

// #endregion

let userMarker, userCircle, zoomed, position, hasLocation;


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

    // Remove any previous user markers
    if (userMarker) {
        map.removeLayer(userMarker);
    }

    // Add the user to the map
    userMarker = L.marker([lat, lon], { icon: userIcon }).addTo(map);

    // Remove accuracy circle if enabled
    if (isDisplayingAccuracy) {
        map.removeLayer(userCircle);    // Remove present circle layer

        // Create circle and add it
        const accuracy = position.coords.accuracy;
        userCircle = L.circle([lat, lon], { radius: accuracy }).addTo(map);
    }

    // Zoom in on the user's location
    if (!zoomed && userCircle) {
        zoomed = map.fitBounds(userCircle.getBounds());
    }

    hasLocation = true;
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

    hasLocation = false;
}

/**
 * Creates a route from the user to the building
 * 
 * @param {any} bLat The desired building's latitude
 * @param {any} bLon The desired building's longitude
 */
export function navigate_user(bLat, bLon) {

    if (hasLocation == true)
    {
        const uLat = position.coords.latitude;
        const uLon = position.coords.longitude;

        // Remove nav route if there already is one
        if (routed) {
            map.removeControl(routed);
        }

        // Create and add the route to the map
        routed = L.Routing.control({
            router: new L.Routing.osrmv1({  }),
            waypoints: [
                L.latLng(uLat, uLon),
                L.latLng(bLat, bLon)
            ],
            draggableWaypoints: false,
            routeWhileDragging: false,
            createMarker: function () { return null; },
            lineOptions: {
                addWaypoints: false,
                styles: [{ color: '#0356fc', weight: 4 }]
            }
        }).addTo(map);

        routed.hide();  // Hides itinerary

        routePos = new Array();
        routePos[0] = bLat;
        routePos[1] = bLon;
    }
}


/**
 * Creates a route for the passed schedule
 * 
 * @param {Float32Array} latitudes The passed array of latitudes
 * @param {Float32Array} longitudes The passed array of longitudes
 * @param {Date} dates The passed array of datetimes
 */
export function navigate_schedule(latitudes, longitudes, dates) {

    // Validate the schedule
    if (latitudes.length > 0 && latitudes.length == longitudes.length) {

        // Navigate the user to the singular building if there is only one building
        if (latitudes.length == 1) {
            navigate_user(latitudes[0], longitudes[0]);
        }
        else {

            if (hasLocation == true) {

                latitudes.forEach((lat, index) => {

                    if (index < latitudes.length) {

                        if (dates[index] > Date.now()) {

                            // Create and add the route to the map [BLUE]
                            L.Routing.control({
                                router: new L.Routing.osrmv1({}),
                                waypoints: [
                                    L.latLng(lat, longitudes[index]),
                                    L.latLng(latitudes[index + 1], longitudes[index + 1])
                                ],
                                draggableWaypoints: false,
                                routeWhileDragging: false,
                                createMarker: function () { return null; },
                                lineOptions: {
                                    addWaypoints: false,
                                    styles: [{ color: '#0356fc', weight: 4 }]
                                }
                            }).addTo(map).hide();

                        }
                        else {

                            // Create and add the route to the map [RED]
                            L.Routing.control({
                                router: new L.Routing.osrmv1({}),
                                waypoints: [
                                    L.latLng(lat, longitudes[index]),
                                    L.latLng(latitudes[index + 1], longitudes[index + 1])
                                ],
                                draggableWaypoints: false,
                                routeWhileDragging: false,
                                createMarker: function () { return null; },
                                lineOptions: {
                                    addWaypoints: false,
                                    styles: [{ color: '#e80909', weight: 4 }]
                                }
                            }).addTo(map).hide();

                        }
                    }
                });

                // Quick and dirty fix for weird rendering issue
                setTimeout(function () { map.setView({ lon: -95.5474, lat: 30.7143 }, 15); }, 100);
            }
        }
    }
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
export function update_info(lat, lon, name, desc, img) {

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
    title.style = "border-bottom: 3px solid black; margin-top: 5px";
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