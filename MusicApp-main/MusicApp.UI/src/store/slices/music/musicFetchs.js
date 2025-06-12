import axios from "axios";
import { musicActions } from "./musicSlice";
// const baseURL = import.meta.env.VITE_API_MUSIC_URL;
const baseURL = import.meta.env.VITE_API_URL+'/music';
export const uploadMusicFetch = (form) => {
  return async (dispatch) => {
    console.log("baseURL: ", baseURL);
    let response = await axios.post(`${baseURL}/api/musics`, form, {
      headers: {
        "Content-Type": "multipart/form-data",
        "Authorization": `Bearer ${localStorage.getItem("TOKEN_KEY")}`,
      },
    });
    dispatch(getMusicsFetch());
    console.log("Music uploaded:", response.data);
  };
};


export const getMusicsFetch = () => {
  return async (dispatch) => {
    try {
      let response = await axios.get(`${baseURL}/api/musics`, {
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("TOKEN_KEY")}`,
        },
      });
      console.log(response.data);
      dispatch(musicActions.setMusicList(response.data));
    } catch (error) {
      console.error("Error fetching music list:", error);
    }
  };
}

export const deleteMusicFetch = (id) => {
  return async (dispatch) => {
    try {
      let response = await axios.delete(`${baseURL}/api/musics/${id}`, {
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("TOKEN_KEY")}`,
        },
      });
      console.log("Music deleted:", response.data);
      dispatch(getMusicsFetch());
    }
    catch (error) {
      console.error("Error deleting music:", error);
    }
  }
}