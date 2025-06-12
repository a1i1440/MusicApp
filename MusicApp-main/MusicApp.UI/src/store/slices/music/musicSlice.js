import { createSlice } from "@reduxjs/toolkit";

let musicSlice = createSlice({
    name: 'musicSlice',
    initialState:{
        musics: []
    },
    reducers: {
        setMusicList: (state, action) => {
            state.musics = action.payload;
        }
    }
})


export const musicActions = musicSlice.actions;
export default musicSlice.reducer;