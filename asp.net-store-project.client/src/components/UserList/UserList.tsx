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
		console.log(data.users);
		console.log(users, "a");
	};

	return <main>
		{users.map((user, idx) => <div key={idx}>{user.isAdmin
			? <>Admin {user.name}</>
			: <>Regular user {user.name}</>}
		</div>)}
	</main>;
}

export default UserList;