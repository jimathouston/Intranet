interface NodeRequire {
    ensure: (paths: string[], callback: (require: NodeRequireFunction) => void) => void
}