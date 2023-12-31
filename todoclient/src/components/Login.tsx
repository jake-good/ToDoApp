import { useState } from "react";
import apiClient from "../api/apiClient";
import useAuth from "../hooks/useAuth";
import { Link, Navigate } from "react-router-dom";
import axios from "axios";

export default function LoginPage() {
  const { setAuth } = useAuth();
  const [success, setSuccess] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await apiClient.post(
        "/users/authenticate",
        { username, password },
        {
          headers: { "Content-Type": "application/json" },
          // withCredentials: true,
        }
      );
      console.log(JSON.stringify(response?.data));
      setAuth({
        accessToken: response.data.jwtToken,
        userId: response.data.userId,
        username,
      });
      setSuccess(true);
    } catch (err: unknown) {
      if (axios.isAxiosError(err)) {
        console.log(err.cause);
      } else {
        console.log(err);
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
          <input
            style={{ marginLeft: "15px" }}
            type="text"
            name="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
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
        <section>
          <button className="login" type="submit">
            Login
          </button>
          <Link to="/signup">
            <p>Sign up</p>
          </Link>
        </section>
      </form>
    </div>
  );
}
