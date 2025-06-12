import axios from "axios";
import { favouritesActions } from "./favouritesSlice";

// const baseURL = import.meta.env.VITE_API_FAVOURITE_URL;
const baseURL = import.meta.env.VITE_API_URL+'/favourite';


export const getFavouritesFetch = () => {
    return async (dispatch) => {
        try {
        let response = await fetch(`${baseURL}/api/favourites`, {
            headers: {
            "Authorization": `Bearer ${localStorage.getItem("TOKEN_KEY")}`,
            },
        });
        if (!response.ok) {
            throw new Error("Network response was not ok");
        }
        let data = await response.json();
        dispatch(favouritesActions.setFavouritesList(data));
        } catch (error) {
        console.error("Error fetching favourites:", error);
        }
    };
}

export const addFavouriteFetch = (musicId) => {
    return async (dispatch) => {
        try {
            let response = await axios.post(`${baseURL}/api/favourites`, musicId, {
                headers: {
                    "Content-Type": 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem("TOKEN_KEY")}`
                }
            })

            console.log(response.data)
            dispatch(getFavouritesFetch())
        } catch (error) {
            
        }
    }
}


export const removeFavouriteFetch = (musicId) => {
    return async (dispatch) => {
        try {
            let response = await axios.delete(`${baseURL}/api/favourites`, {
                headers: {
                    "Content-Type": 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem("TOKEN_KEY")}`
                },
                data: musicId
            })

            console.log(response.data)
            dispatch(getFavouritesFetch())
        } catch (error) {
            
        }
    }
}