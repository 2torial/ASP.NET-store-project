import { useEffect, useState } from 'react';
import './UserList.css';

interface UserListComponentData {
	users: User[];
}

interface User {
	name: string;
    isAdmin: boolean;
}

function UserList() {
    const [users, setUsers] = useState<User[]>([]);

    useEffect(() => {
		collectUsersData();
    }, []);

    const collectUsersData = async () => {
		const response = await fetch('/api/admin/users');
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
		}
		const data: UserListComponentData = await response.json();
		setUsers(data.users);
	};

	return <main>
		<table>
			<tr>
				<th>Username</th>
				<th>Privileges</th>
			</tr>
			{users.map((user, idx) => <tr key={idx}>{user.isAdmin
				? <><td>{user.name}</td><td>Admin</td></>
				: <><td>{user.name}</td><td>Regular</td></>}
			</tr>)}
		</table>
	</main>;
}

export default UserList;