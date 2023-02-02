import { useState, useEffect } from "react"
import BikeCard from "./BikeCard"
import { getBikes }  from "../modules/bikeManager"

export default function BikeList({setDetailsBikeId}) {
    const [bikes, setBikes] = useState([])

    const getAllBikes = () => {
        //implement functionality here...
        getBikes().then(bikes => setBikes(bikes));
    }

    useEffect(() => {
        getAllBikes()
    }, [])
    return (
        <>
        <h2>Bikes</h2>
        <div>
          {bikes.map((bike) => (
            <BikeCard bike={bike} setDetailsBikeId={setDetailsBikeId} key={bike.id} />
          ))}  
        </div>
        
        </>
    )
};
