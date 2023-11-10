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
            <button onClick={() => setAuth({})}>Logout</button>
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
