import { Route, Routes } from "react-router-dom";
import Layout from "./Layout";
import Login from "./Login";
import Tasks from "./Tasks";
import RequireAuth from "./RequireAuth";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route path="login" element={<Login />} />
        <Route element={<RequireAuth />}>
          <Route path="tasks" element={<Tasks />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
