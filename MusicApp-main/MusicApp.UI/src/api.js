import axios from "axios";

const api = axios.create({
  // baseURL: import.meta.env.VITE_API_IDENTITY_URL,
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    "Content-Type": "application/json"
  }
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("TOKEN_KEY");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config
  },
  (error) => {
    return Promise.reject(error);
  }
)

export const setupAxiosInterceptors = (navigate) => {
  api.interceptors.response.use(
    (response) => response,

    (error) => {
      let errorMessage = '';
      if (!error.response) {

        errorMessage = 'Network error! Probably API not responding. Make sure you have active internet connection';
      } else {
        switch (error.response.status) {
          case 400:
            errorMessage = `Bad Request: ${error.response.data}`
            break;
          case 401:
            errorMessage = `Unauthorized: ${error.response.data}`
            localStorage.removeItem("TOKEN_KEY");
            navigate('/login');
            break;
          case 403:
            if (error.response.data.includes("Email not confirmed")) {
              navigate('/login', { state: { needsEmailConfirmation: true } })
              errorMessage = `Forbidden: Email is not confirmed`
            }
            else
              errorMessage = `Forbidden: You don't have permission`;
            break;
          case 404:
            if (error.response.data) {
              errorMessage = error.response.data
            } else
              errorMessage = "Not Found: The resource doesn't exist.";
            break;
          case 500:
            errorMessage = "Server Error: Try again later.";
            break;
          default:
            errorMessage = `Error: ${error.response.data}`;
        }
      }
      console.log(errorMessage)
      return Promise.reject(error);
    }
  );
};

export default api;
