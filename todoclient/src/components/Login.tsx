import { useState } from "react";
import apiClient from "../api/apiClient";
import useAuth from "../hooks/useAuth";
import { Link, Navigate } from "react-router-dom";

export default function LoginPage() {
  const { setAuth } = useAuth();
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await apiClient.post("/users/login", e.target, {
        headers: { "Content-Type": "application/json" },
        // withCredentials: true,
      });
      console.log(JSON.stringify(response?.data));
      const accessToken = response.data.token;
      setAuth({ accessToken });
      setSuccess(true);
    } catch (err) {
      if (!err?.response) {
        console.log("No server response");
      } else {
        console.log(err.response);
      }
    }
  };

  return success ? (
    <Navigate to="/tasks" />
  ) : (
    <div>
      <form className="login-container" action="post" onSubmit={handleSubmit}>
        <label>
          Username:
          <input style={{ marginLeft: "15px" }} type="text" name="username" />
        </label>
        <label>
          Password:
          <input style={{ marginLeft: "15px" }} type="text" name="password" />
        </label>
        <section>
          <button className="login" type="submit">
            Login
          </button>
          <Link to="/signup">
            <a>Sign up</a>
          </Link>
        </section>
      </form>
    </div>
  );
}