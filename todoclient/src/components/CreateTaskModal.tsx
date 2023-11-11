import { useState } from "react";
import { Task } from "./Tasks";

const CreateTaskModal = ({ createTask, cancel }) => {
  const [task, setTask] = useState<Task>({});

  const handleSubmit = (e) => {
    e.preventDefault();
    createTask(task);
  };

  return (
    <form className="create-task-modal" onSubmit={handleSubmit}>
      <h2 style={{ borderBottom: "1px black solid", marginBottom: "25px" }}>
        Create new Task
      </h2>
      <label>
        Title:
        <input
          type="text"
          value={task.title}
          onChange={(e) => setTask({ ...task, title: e.target.value })}
        ></input>
      </label>
      <label>
        Description:
        <textarea
          value={task.description}
          onChange={(e) => setTask({ ...task, description: e.target.value })}
        ></textarea>
      </label>
      <button type="submit">Submit</button>
      <button onClick={cancel}>Cancel</button>
    </form>
  );
};

export default CreateTaskModal;
