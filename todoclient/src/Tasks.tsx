import { useEffect, useState } from "react";
import useApiPrivate from "./hooks/useApiPrivate";

type Task = {
  id: number;
  title: string;
  description: string;
  userId: number;
};

export default function Tasks() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const apiClientPrivate = useApiPrivate();

  useEffect(() => {
    getTasks();
  }, []);

  const getTasks = async () => {
    try {
      const res = await apiClientPrivate.get("/ToDoItems");
      console.log(res.data);
      setTasks(res.data);
    } catch (e) {
      console.log(e);
    }
  };

  const createTask = () => {
    const task: Task = {
      id: 0,
      userId: 4,
      title: "This is a task",
      description: "This is a task description",
    };
    try {
      apiClientPrivate.post("/ToDoItems", task).then(() => getTasks());
    } catch (e) {
      console.log(e);
    }
  };

  const deleteTask = (id: number) => {
    try {
      apiClientPrivate.delete(`/ToDoItems/${id}`).then(() => getTasks());
    } catch (e) {
      console.log(e);
    }
  };

  return (
    <>
      <button
        style={{ bottom: "20px", left: "20px", position: "fixed" }}
        onClick={createTask}
      >
        +
      </button>
      <div className="tasks-container">
        <div className="tasks-column">
          {tasks.map((task) => (
            <div className="task">
              <h1>{task.title}</h1>
              <p>{task.description}</p>
              <button onClick={() => deleteTask(task.id)}>delete</button>
            </div>
          ))}
        </div>
      </div>
    </>
  );
}
