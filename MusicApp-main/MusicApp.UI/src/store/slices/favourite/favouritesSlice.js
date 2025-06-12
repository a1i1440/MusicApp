import { createSlice } from "@reduxjs/toolkit";

const favouritesSlice = createSlice({
    name: 'favouritesSlice',
    initialState: {
        favourites: []
    },
    reducers: {
        setFavouritesList: (state, action) => {
            state.favourites = action.payload;
        },
    }
});

export const favouritesActions = favouritesSlice.actions;
export default favouritesSlice.reducer;