import React, { useEffect, useState } from "react";
import axios from "axios";

function App() {
  const [users, setUsers] = useState([]);

  async function fetchUsers() {
    try {
      const response = await axios.get("https://localhost:7189/api/users");
      return response.data;
    } catch (error) {
      console.error("Error fetching users:", error);
      return [];
    }
  }

  useEffect(() => {
    async function fetchData() {
      const data = await fetchUsers();
      setUsers(data);
    }

    fetchData();
  }, []);

  return (
    <div>
      {users.map((user) => (
        <div>
          <p>{"UserId: " + user.userId}</p>
          <p>{"Username: " + user.username}</p>
          <p>{"Email: " + user.email}</p>
          {user.todoItems.map((item) => (
            <div>
              <p>{"Todo id : " + item.Id}</p>
              <p>{"Todo title:" + item.title}</p>
              <p>{"Todo description: " + item.description}</p>
            </div>
          ))}
        </div>
      ))}
    </div>
  );
}

export default App;
