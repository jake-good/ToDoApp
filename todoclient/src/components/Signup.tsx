import { useState } from "react";
import apiClient from "../api/apiClient";
import { Navigate } from "react-router-dom";

const SignUp = () => {
  const [username, setUser] = useState("");
  const [password, setPassword] = useState("");
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await apiClient.post(
        "/users/signup",
        { username, password },
        {
          headers: { "Content-Type": "application/json" },
        }
      );
      console.log(res);
      setSuccess(true);
    } catch (err) {
      console.log(err);
      setError(err.message);
    }
  };

  return success ? (
    <Navigate to="/login" />
  ) : (
    <>
      <form className="login-container" action="post" onSubmit={handleSubmit}>
        <label>
          Username:
          <input
            style={{ marginLeft: "15px" }}
            type="text"
            name="username"
            value={username}
            onChange={(e) => setUser(e.target.value)}
          />
        </label>
        <label>
          Password:
          <input
            style={{ marginLeft: "15px" }}
            type="text"
            name="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </label>
        <button className="login" type="submit">
          Signup
        </button>
      </form>
      <p>{error}</p>
    </>
  );
};

export default SignUp;
