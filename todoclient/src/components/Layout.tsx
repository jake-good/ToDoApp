import { Link, Outlet } from "react-router-dom";
import useAuth from "../hooks/useAuth";

const Layout = () => {
  const { auth, setAuth } = useAuth();
  return (
    <div className="base">
      <div className="header">
        <h1>What To Do?</h1>
        <div className="profile">
          {!auth?.accessToken ? (
            <Link to="/Login">
              <button className="login-nav">Login</button>
            </Link>
          ) : (
            <div
              style={{
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
              }}
            >
              <p>Logged in as: </p>
              <p style={{ padding: "10px", fontWeight: "bold" }}>
                {auth?.username}
              </p>
              <button
                onClick={() =>
                  setAuth({ accessToken: "", userId: "", username: "" })
                }
              >
                Logout
              </button>
            </div>
          )}
        </div>
      </div>
      <div className="body">
        <Outlet></Outlet>
      </div>
    </div>
  );
};

export default Layout;
