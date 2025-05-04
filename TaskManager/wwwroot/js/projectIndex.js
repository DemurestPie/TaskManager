'use strict';

const assignTaskModalDOM = document.getElementById('assignTaskModal');
const assignTaskModal = new bootstrap.Modal(assignTaskModalDOM);

// For debugging purposes
console.log('Project Index JS loaded');

// Fetches data from the restful api
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('a.open-assign-modal').forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            const projectId = this.getAttribute('asp-route-id') || this.dataset.projectId;

            fetch('/api/taskapi')
                .then(response => response.json())
                .then(data => {
                    const tasks = data.$values ?? []; // safely get task array
                    populateTasks(tasks, projectId);
                    assignTaskModal.show();
                })
                .catch(error => {
                    console.error('Error loading tasks:', error);
                });
        });
    });
});

// Populates the task table in the modal
function populateTasks(tasks, projectId) {
    const tbody = document.getElementById('taskTableBody');
    tbody.innerHTML = '';

    tasks.forEach(task => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${task.title}</td>
            <td>${task.status}</td>
            <td>${task.user?.name ?? "(none)"}</td>
            <td>
                <button class="btn btn-sm btn-success assign-btn"
                        data-task-id="${task.id}"
                        data-project-id="${projectId}">
                    Assign
                </button>
            </td>
        `;
        tbody.appendChild(tr);
    });

    document.querySelectorAll('.assign-btn').forEach(button => {
        button.addEventListener('click', async function () {
            const taskId = this.dataset.taskId;
            const projectId = this.dataset.projectId;

            const result = await assignTaskToProject(taskId, projectId);
            if (result.success) {
                alert('Task assigned!');
                assignTaskModal.hide();
                window.location.reload();
            } else {
                alert('Error: ' + result.message);
            }
        });
    });
}

// Assigns the task to the project
async function assignTaskToProject(taskId, projectId) {
    try {
        const response = await fetch(`/api/projectapi/assign/${projectId}?taskId=${taskId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.status === 204) {
            return { success: true };
        }

        return await response.json();
    } catch (err) {
        console.error(err);
        return { success: false, message: 'AJAX error' };
    }
}
