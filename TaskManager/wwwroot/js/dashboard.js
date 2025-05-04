'use strict';

// Fetches data from the restful api
document.addEventListener('DOMContentLoaded', () => {
    fetch('/api/dashboardapi')
        .then(res => res.json())
        .then(data => {
            renderCards(data);
            renderRecentTasks(data.recentTasks?.$values || []);
        })
        .catch(err => console.error('Dashboard load error:', err));
});

// Draws the cards on the dashboard
function renderCards(data) {
    const container = document.getElementById('dashboardStats');
    container.innerHTML = `
        ${createCard("Tasks To Do", data.tasksToDo, "bg-warning")}
        ${createCard("Tasks Completed", data.tasksDone, "bg-success")}
        ${createCard("Projects To Do", data.projectsToDo, "bg-primary")}
        ${createCard("Projects Completed", data.projectsDone, "bg-secondary")}
    `;
}

// Creates the cards
function createCard(label, value, colorClass) {
    return `
        <div class="col-md-3">
            <div class="card text-white ${colorClass} text-center shadow rounded-3">
                <div class="card-body">
                    <h5 class="card-title">${label}</h5>
                    <h2>${value}</h2>
                </div>
            </div>
        </div>
    `;
}

// Draws the recent tasks table
function renderRecentTasks(tasks) {
    const tbody = document.getElementById('recentTasksBody');
    tbody.innerHTML = '';

    tasks.forEach(task => {
        tbody.innerHTML += `
            <tr>
                <td>${task.title}</td>
                <td>${task.status}</td>
                <td>${task.priority}</td>
                <td>${task.user ?? "(unassigned)"}</td>
            </tr>
        `;
    });
}
