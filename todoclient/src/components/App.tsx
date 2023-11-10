import { Route, Routes } from "react-router-dom";
import Layout from "./Layout";
import Login from "./Login";
import Tasks from "./Tasks";
import RequireAuth from "./RequireAuth";
import SignUp from "./Signup";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route path="login" element={<Login />} />
        <Route path="signup" element={<SignUp />} />
        <Route element={<RequireAuth />}>
          <Route path="tasks" element={<Tasks />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
