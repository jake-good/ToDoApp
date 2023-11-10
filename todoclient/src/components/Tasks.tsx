import { useEffect, useState } from "react";
import useApiPrivate from "../hooks/useApiPrivate";
import useAuth from "../hooks/useAuth";
import CreateTaskModal from "./CreateTaskModal";

export type Task = {
  id: number;
  title: string;
  description: string;
  userId: number;
};

export default function Tasks() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [showModal, setShowModal] = useState(false);
  const apiClientPrivate = useApiPrivate();
  const { auth } = useAuth();

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

  const createTask = (task: Task) => {
    try {
      task.userId = auth.userId;
      apiClientPrivate.post("/ToDoItems", task).then(() => getTasks());
      setShowModal(false);
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
      {showModal && <CreateTaskModal createTask={createTask} />}
      <button
        style={{ bottom: "20px", left: "20px", position: "fixed" }}
        onClick={() => setShowModal(true)}
      >
        +
      </button>
      <div className={(showModal ? "blur" : "") + " tasks-container"}>
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
