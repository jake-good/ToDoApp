import { useState } from "react";
import { Task } from "./Tasks";

const CreateTaskModal = ({ createTask }) => {
  const [task, setTask] = useState<Task>({});

  const handleSubmit = () => {
    createTask(task);
  };

  return (
    <form className="create-task-modal" onSubmit={handleSubmit}>
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
    </form>
  );
};

export default CreateTaskModal;
