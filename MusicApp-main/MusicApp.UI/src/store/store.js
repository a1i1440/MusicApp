import { configureStore } from "@reduxjs/toolkit";
import musicSlice from "./slices/music/musicSlice";
import accountSlice from "./slices/account/accountSlice";
import favouritesSlice from "./slices/favourite/favouritesSlice";

const store = configureStore({
    reducer: {
        music: musicSlice,
        account: accountSlice,
        favourite: favouritesSlice
    }
})

export default store;