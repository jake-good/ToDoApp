import { Link, Outlet } from "react-router-dom";

const Layout = () => {
  return (
    <div className="base">
      <div className="header">
        <h1>What To Do?</h1>
        <div className="profile">
          <Link to="/Login">
            <button className="login-nav">Login</button>
          </Link>
        </div>
      </div>
      <div className="body">
        <Outlet></Outlet>
      </div>
    </div>
  );
};

export default Layout;
