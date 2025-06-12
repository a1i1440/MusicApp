import { createSlice } from "@reduxjs/toolkit";

let accountSlice = createSlice({
    name:'accountSlice',
    initialState: {
        username: ''
    },
    reducers: {

    }
})

export const accountActions = accountSlice.actions;
export default accountSlice.reducer;