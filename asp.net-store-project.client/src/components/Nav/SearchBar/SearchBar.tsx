import "./SearchBar.css";

interface SearchBarProps {
    searchBarRef: React.MutableRefObject<null>
    focusSearchBar: () => void;
    unfocusSearchBar: () => void;
}

function SearchBar({ searchBarRef, focusSearchBar, unfocusSearchBar }: SearchBarProps) {
    return <div className="search-bar-section">
        <form autoComplete="off">
            <search className="search-bar" ref={searchBarRef} onFocus={focusSearchBar} onBlur={unfocusSearchBar}>
                <input className="input-area" type="text" name="SearchBar" placeholder="Search" />
                <input className="search-button" type="image" src="https://placehold.co/40x40" id="search-link" alt="magnifier" />
            </search>
        </form>
    </div>;
}

export default SearchBar;