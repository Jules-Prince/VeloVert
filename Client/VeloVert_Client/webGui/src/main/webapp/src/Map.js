import React from "react";
import {MapContainer, TileLayer} from "react-leaflet";
import RoutineMachine from "./RoutingMachine";

const Map = (props) => {

    const center = [43.7, 7.25];
    return (
        <div className="map">
            <MapContainer center={center} zoom={13} scrollWheelZoom={false}>
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <RoutineMachine/>
            </MapContainer>
        </div>
    );
};

export default Map;