FIFO := /tmp/rendrlog
PROJECT := RendrTest

.PHONY: run clean

run:
	@echo "Creating FIFO..."
	@rm -f $(FIFO)
	@mkfifo $(FIFO)

	@echo "Opening kitty for log viewer..."
	@kitty --title "Pipe Log" sh -c "while true; do cat $(FIFO); sleep 0.02; done" &

	@sleep 0.5

	@echo "Running dotnet project..."
	@kitty @ set-font-size 8
	@dotnet watch run --project $(PROJECT)

clean:
	@rm -f $(FIFO)
