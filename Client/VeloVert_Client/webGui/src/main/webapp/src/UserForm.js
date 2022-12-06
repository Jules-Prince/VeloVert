import React, {useState} from 'react';
import logo from './logoVeloVert.png'
import './UserForm.css'
import axios from 'axios'
function UserForm() {
    const [depart, setDepart] = useState('');
    const [arrivee, setArrivee] = useState('');
    const [response, setResponse] = useState(null);
    const [gpsPoint,setGpsPoint] = useState([]);

    function handleDepartChange(event) {
        setDepart(event.target.value);
    }

    function handleArriveeChange(event) {
        setArrivee(event.target.value);
    }

    async function handleSubmit(event) {
        console.log("coucou")
        const itineary = {
            departure: depart,
            arrival: arrivee
        };
        console.log(itineary);
        /*axios.post("http://localhost:8084/route",itineary).then(res =>{
            setGpsPoint(res.data);

            console.log(gpsPoint);
        });*/


    }

    return (

        <div className="parentStyles">
            <div className="logoStyle">
                <img src={logo} alt="logoVeloVert" />
            </div>
            <div >
                <div className="userForm">
                    <form onSubmit={handleSubmit}>
                        <div>
                            <label htmlFor="depart">Départ : </label>
                            <input type="text" id="depart" value={depart} onChange={handleDepartChange}/>
                        </div>
                        <div>
                            <label htmlFor="arrivee">Arrivée : </label>
                            <input type="text" id="arrivee" value={arrivee} onChange={handleArriveeChange}/>
                        </div>
                        <button type="submit">Send</button>
                    </form>
                </div>
            </div>
        </div>


    );
}

export default UserForm;