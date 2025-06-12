import api from "../../../api"


export const registerFetch = (user) => {
    return async (dispatch) => {
        console.log(user)
        const response = await api.post('/identity/api/account/register', user);
        console.log(response.data)
    }
}

export const loginFetch = (user) => {
    return async (dispatch) => {
        const response = await api.post('/identity/api/account/login', user);
        console.log(response.data)
        localStorage.setItem("TOKEN_KEY", response.data.token);
    }
}