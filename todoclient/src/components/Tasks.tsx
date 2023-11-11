import { useEffect, useState } from "react";
import useApiPrivate from "../hooks/useApiPrivate";
import useAuth from "../hooks/useAuth";
import CreateTaskModal from "./CreateTaskModal";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashCan } from "@fortawesome/free-regular-svg-icons";
import { faAdd } from "@fortawesome/free-solid-svg-icons";

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

  const createTask = async (task: Task) => {
    try {
      task.userId = auth.userId;
      await apiClientPrivate.post(`/ToDoItems`, task);
      setShowModal(false);
      getTasks();
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
      {showModal && (
        <CreateTaskModal
          createTask={createTask}
          cancel={() => setShowModal(false)}
        />
      )}
      <button
        style={{
          bottom: "20px",
          left: "20px",
          position: "fixed",
        }}
        onClick={() => setShowModal(true)}
      >
        New Task
        <FontAwesomeIcon style={{ paddingLeft: "10px" }} icon={faAdd} />
      </button>
      <div className={(showModal ? "blur" : "") + " tasks-container"}>
        <div className="tasks-column">
          {tasks.map((task) => (
            <div key={task.id.toString()} className="task">
              <h1>{task.title}</h1>
              <p>{task.description}</p>
              <button
                className="delete-task"
                onClick={() => deleteTask(task.id)}
              >
                <FontAwesomeIcon icon={faTrashCan} />
              </button>
            </div>
          ))}
        </div>
      </div>
    </>
  );
}
