
import './App.css';
import 'leaflet/dist/leaflet.css';
import Map from "./Map";
import UserForm from "./UserForm";

function App() {

    const position = [51.505, -0.09]

    return (
        <div className="App">
            <UserForm/>
            <Map/>
        </div>
    );
}

export default App;
