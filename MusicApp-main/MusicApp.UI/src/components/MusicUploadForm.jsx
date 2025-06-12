import { useState, useRef } from "react";
import { useDispatch } from "react-redux";
import { uploadMusicFetch } from "../store/slices/music/musicFetchs";

export default function MusicUploadForm() {
    const [musicName, setMusicName] = useState("");
    const [musicPhoto, setMusicPhoto] = useState(null);
    const [musicFile, setMusicFile] = useState(null);
    const dispatch = useDispatch();
    const musicFileRef = useRef();
    const musicPhotoRef = useRef();

    const handleUpload = () => {
        const formData = new FormData();
        formData.append("Name", musicName);
        formData.append("PhotoFile", musicPhoto);
        formData.append("MusicFile", musicFile);
        dispatch(uploadMusicFetch(formData));
    };

    return (
        <div style={{ padding: "1rem", backgroundColor: "#121212", color: "white", fontFamily: "sans-serif" }}>
            <h1 style={{ fontSize: "2rem", color: "#1DB954" }}>Music App</h1>

            <div style={{ display: "flex", gap: "1rem", marginTop: "1rem", flexWrap: "wrap" }}>
                <input
                    type="text"
                    placeholder="Music Name"
                    value={musicName}
                    onChange={(e) => setMusicName(e.target.value)}
                    style={{ padding: "0.5rem", backgroundColor: "#222", border: "1px solid #333", color: "white", flex: 1 }}
                />
                <input
                    type="file"
                    ref={musicPhotoRef}
                    accept="image/*"
                    onChange={(e) => setMusicPhoto(e.target.files[0])}
                    style={{ padding: "0.5rem", backgroundColor: "#222", color: "white" }}
                />
                <input
                    type="file"
                    ref={musicFileRef}
                    accept="audio/*"
                    onChange={(e) => setMusicFile(e.target.files[0])}
                    style={{ padding: "0.5rem", backgroundColor: "#222", color: "white" }}
                />
                <button
                    onClick={handleUpload}
                    style={{ backgroundColor: "#1DB954", border: "none", padding: "0.5rem 1rem", color: "white" }}
                >
                    Upload
                </button>
            </div>
        </div>
    );
}
