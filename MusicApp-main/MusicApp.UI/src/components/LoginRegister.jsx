import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { loginFetch, registerFetch } from "../store/slices/account/accountFetchs";
import { useNavigate } from "react-router-dom";

const LoginRegister = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const handleLogin = async () => {
    await dispatch(loginFetch({ Username: username, Password: password }));
    navigate("/");
  };

  const handleRegister = () => {
    dispatch(registerFetch({ Username: username, Password: password}))
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-4">
          <div className="card shadow-sm">
            <div className="card-body">
              <h3 className="card-title text-center mb-4">Login / Register</h3>
              <div className="mb-3">
                <input
                  type="text"
                  className="form-control"
                  placeholder="Username"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                />
              </div>
              <div className="mb-3">
                <input
                  type="password"
                  className="form-control"
                  placeholder="Password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>
              <div className="d-flex justify-content-between">
                <button className="btn btn-primary w-45" onClick={handleLogin}>
                  Login
                </button>
                <button
                  className="btn btn-success w-45"
                  onClick={handleRegister}
                >
                  Register
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginRegister;
