import { default as SearchBar } from './SearchBar';
import './Nav.css';
import { Link, useNavigate } from 'react-router-dom';
import { useRef } from 'react';
import { IdentityPolicy } from '../../shared/StoreEnum/IdentityPolicy';

interface NavProps {
	updateUserIdentity(): void,
	userIdentity: IdentityPolicy
}

export function Nav({ updateUserIdentity, userIdentity }: NavProps) {
	const navigate = useNavigate();
	const menuRef = useRef(null);
	const profileIconRef = useRef(null);
	const openMenu = () => {
		(menuRef.current! as HTMLElement).classList.add("opened");
		(profileIconRef.current! as HTMLElement).classList.add("activated");
	}
	const hideMenu = () => {
		(menuRef.current! as HTMLElement).classList.remove("opened");
		(profileIconRef.current! as HTMLElement).classList.remove("activated");
	}

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
					<span className="cart-icon fa fa-shopping-cart" />
				</Link>
				<span ref={profileIconRef} onMouseOver={openMenu} className="profile-icon fa fa-cogs" />
			</div>
		</div>
		<div className="menu" ref={menuRef}> {userIdentity === IdentityPolicy.AnonymousUser
			? <>
				{[...Array(6).keys()].map((i) => <div className="empty-space" key={i}></div>)}
				<div className="option-section">
					<Link to="/sign-in">Sign in</Link>
				</div>
				<div className="option-section">
					<Link to="/sign-up">Sign up</Link>
				</div>
			</>
			: <> {userIdentity === IdentityPolicy.AdminUser
				? <>
					{[...Array(5).keys()].map((i) => <div className="empty-space" key={i}></div>)}
					<div className="option-section">
						<Link to="/admin/users">Users</Link>
					</div>
					<div className="option-section">
						<Link to="/orders">Orders</Link>
					</div>
					<div className="option-section">
						<a onClick={signOut}>Sign out</a>
					</div>
				</>
				: <>
					{[...Array(6).keys()].map((i) => <div className="empty-space" key={i}></div>)}
					<div className="option-section">
						<Link to="/orders">Orders</Link>
					</div>
					<div className="option-section">
						<a onClick={signOut}>Sign out</a>
					</div>
				</>}
			</>}
		</div>
	</nav>;
}

export default Nav;