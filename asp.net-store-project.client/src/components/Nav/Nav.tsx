import { default as SearchBar } from './SearchBar';
import './Nav.css';
import { Link, useNavigate } from 'react-router-dom';
import { useRef } from 'react';

interface NavProps {
	updateUserIdentity(): void,
	userIdentity: Identity
}
type Identity = "Anonymous" | "User" | "Admin";

export function Nav({ updateUserIdentity, userIdentity }: NavProps) {
	const navigate = useNavigate();
	const menuRef = useRef(null);
	const openMenu = () => (menuRef.current! as HTMLElement).classList.add("opened");
	const hideMenu = () => (menuRef.current! as HTMLElement).classList.remove("opened");

	const signOut = async (event: React.MouseEvent) => {
		event.preventDefault();
		const response = await fetch('/api/account/logout');
		alert(await response.text());
		updateUserIdentity();
		navigate("/");
	}

    return <nav onMouseLeave={hideMenu}>
		<div className="navigation">
			<div className="logo-section">
				<Link to="/">
					<img src="https://placehold.co/120x50" alt="logo" />
				</Link>
			</div>
			<div className="empty-space-section"></div>
			<SearchBar />
			<div className="menu-section">
				<Link to="/basket">
					<img src="https://placehold.co/40x40" alt="basket" />
				</Link>
				<img onMouseOver={openMenu} src="https://placehold.co/40x40" alt="profile" />
			</div>
		</div>
		<div className="menu" ref={menuRef}> {userIdentity === "Anonymous"
			? <>
				{[...Array(6).keys()].map((i) => <div className="empty-space" key={i}></div>)}
				<div className="option-section">
					<Link to="/sign-in">Sign in</Link>
				</div>
				<div className="option-section">
					<Link to="/sign-up">Sign up</Link>
				</div>
			</>
			: <> {userIdentity === "Admin"
				? <>
					{[...Array(4).keys()].map((i) => <div className="empty-space" key={i}></div>)}
					<div className="option-section">
						<Link to="/admin/users">Users</Link>
					</div>
					<div className="option-section">
						<Link to="/admin/orders">Orders</Link>
					</div>
					<div className="option-section">
						<Link to="/admin/items">Store items</Link>
					</div>
					<div className="option-section">
						<a onClick={signOut}>Sign out</a>
					</div>
				</>
				: <>
					{[...Array(7).keys()].map((i) => <div className="empty-space" key={i}></div>)}
					<div className="option-section">
						<a onClick={signOut}>Sign out</a>
					</div>
				</>}
			</>}
		</div>
	</nav>;
}

export default Nav;